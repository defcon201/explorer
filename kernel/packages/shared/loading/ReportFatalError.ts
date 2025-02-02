declare var window: Window & { Rollbar: any }
import { action } from 'typesafe-actions'
import {
  COMMS_COULD_NOT_BE_ESTABLISHED,
  fatalError,
  ExecutionLifecycleEvent,
  ExecutionLifecycleEventsList,
  MOBILE_NOT_SUPPORTED,
  NETWORK_MISMATCH,
  NEW_LOGIN,
  NO_WEBGL_COULD_BE_CREATED,
  NOT_INVITED,
  AVATAR_LOADING_ERROR
} from './types'
import { StoreContainer } from 'shared/store/rootTypes'
import Html from '../Html'
import { trackEvent } from '../analytics'

declare const globalThis: StoreContainer

export let aborted = false

export function bringDownClientAndShowError(event: ExecutionLifecycleEvent) {
  if (aborted) {
    return
  }
  const body = document.body
  const container = document.getElementById('gameContainer')
  container!.setAttribute('style', 'display: none !important')

  Html.hideProgressBar()

  body.setAttribute('style', 'background-image: none !important;')

  const targetError =
    event === COMMS_COULD_NOT_BE_ESTABLISHED
      ? 'comms'
      : event === NOT_INVITED
      ? 'notinvited'
      : event === NO_WEBGL_COULD_BE_CREATED
      ? 'notsupported'
      : event === MOBILE_NOT_SUPPORTED
      ? 'nomobile'
      : event === NEW_LOGIN
      ? 'newlogin'
      : event === NETWORK_MISMATCH
      ? 'networkmismatch'
      : event === AVATAR_LOADING_ERROR
      ? 'avatarerror'
      : 'fatal'

  globalThis.globalStore && globalThis.globalStore.dispatch(fatalError(targetError))
  Html.showErrorModal(targetError)
  aborted = true
}

export type FatalErrorInfo = {
  type: string
  message: string
  stack?: string
  sagaStack?: string
  filename?: string
}

export function ReportFatalError(event: ExecutionLifecycleEvent, errorInfo?: FatalErrorInfo) {
  bringDownClientAndShowError(event)
  if (ExecutionLifecycleEventsList.includes(event)) {
    return globalThis.globalStore && globalThis.globalStore.dispatch(action(event))
  }
  trackEvent('generic_error', {
    message: event,
    errorInfo
  })
}

export function ReportSceneError(message: string, error: any) {
  const eventData = {
    error,
    scene: true,
    message,
    position: new URLSearchParams(location.search).get('position')
  }
  trackEvent('scene_error', eventData)
  if (window.Rollbar) {
    window.Rollbar.error(message, eventData)
  }
}

export function ReportRendererInterfaceError(message: string, error: any) {
  const eventData = {
    error,
    message,
    rendererInterface: true,
    position: new URLSearchParams(location.search).get('position')
  }
  trackEvent('renderer_interface_error', eventData)
  if (window.Rollbar) {
    window.Rollbar.error(message, eventData)
  }
}
