import future from 'fp-future'
import { parseUrn } from '@dcl/urn-resolver'

declare const globalThis: any

export type DclRenderer = {}

export type LoadRendererResult = {
  DclRenderer: DclRenderer
  createUnityInstance: (
    canvas: HTMLCanvasElement,
    config: any,
    onProgress?: (progress: number) => void
  ) => Promise<UnityGame>
  baseUrl: string
}

export type UnityGame = {
  Module: {
    /** this handler can be overwritten, return true to stop error propagation */
    errorHandler?: (message: string, filename: string, lineno: number) => boolean
  }
  SendMessage(object: string, method: string, args: number | string): void
  SetFullscreen(): void
  Quit(): Promise<void>
}

// TODO: return type DclRenderer
async function injectRenderer(baseUrl: string): Promise<LoadRendererResult> {
  const scriptUrl = new URL('index.js', baseUrl).toString()
  await injectScript(scriptUrl)

  if (typeof globalThis.createUnityInstance === 'undefined') {
    throw new Error('Error while loading createUnityInstance from ' + scriptUrl)
  }

  if (typeof globalThis.DclRenderer === 'undefined') {
    throw new Error('Error while loading the renderer from ' + scriptUrl)
  }

  return {
    DclRenderer: globalThis.DclRenderer,
    createUnityInstance: globalThis.createUnityInstance,
    baseUrl
  }
}

async function loadDefaultRenderer(): Promise<LoadRendererResult> {
  // PAY ATTENTION:
  //  Whenever we decide to not bundle the renderer anymore and have independant
  //  release cycles for the explorer, replace this whole function by the following commented line
  //
  // > return loadRendererByBranch('master')

  function getRendererArtifactsRoot() {
    // This function is used by preview, instead of using "." as root,
    // preview uses '/@/artifacts'
    if (typeof globalThis.RENDERER_ARTIFACTS_ROOT === 'undefined') {
      throw new Error('RENDERER_ARTIFACTS_ROOT is undefined')
    } else {
      return new URL(globalThis.RENDERER_ARTIFACTS_ROOT, document.location.toString()).toString()
    }
  }

  // Load the embeded renderer from the artifacts root folder
  return injectRenderer(getRendererArtifactsRoot())
}

async function loadRendererByBranch(branch: string): Promise<LoadRendererResult> {
  const baseUrl = `https://renderer-artifacts.decentraland.org/branch/${branch}/`
  return injectRenderer(baseUrl)
}

export async function loadUnity(urn?: string): Promise<LoadRendererResult> {
  if (!urn) {
    return loadDefaultRenderer()
  } else {
    const parsedUrn = await parseUrn(urn)

    if (!parsedUrn) {
      throw new Error('An invalid urn was provided for the renderer')
    }

    // urn:decentraland:off-chain:renderer-artifacts:${branch}
    if (parsedUrn.type === 'off-chain' && parsedUrn.registry === 'renderer-artifacts') {
      return loadRendererByBranch(parsedUrn.id)
    }

    throw new Error('It was impossible to resolve a renderer for the URN "' + urn + '"')
  }
}

async function injectScript(url: string) {
  const theFuture = future<Event>()
  const theScript = document.createElement('script')
  theScript.src = url
  theScript.async = true
  theScript.type = 'application/javascript'
  theScript.addEventListener('load', theFuture.resolve)
  theScript.addEventListener('error', (e) => theFuture.reject(e.error || (e as any)))
  document.body.appendChild(theScript)
  return theFuture
}
