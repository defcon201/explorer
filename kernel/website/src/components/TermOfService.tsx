import * as React from 'react'

import './EthLogin.css'
export interface TermOfServiceProps {
  checked: boolean
  onChange: any
}
export const TermOfService: React.FC<TermOfServiceProps> = (props) => (
  <div class="eth-login-tos">
    <input
      type="checkbox"
      id="agree-check"
      className="eth-login-tos-agree"
      checked={props.checked}
      onChange={props.onChange}
    />
    <label htmlFor="agree-check" className="eth-login-tos-label">
      I am of legal age and I have read and agree to the{' '}
      <a href="https://decentraland.org/terms" target="_blank" rel="noopener noreferrer">
        Terms of Service
      </a>{' '}
      and{' '}
      <a href="https://decentraland.org/privacy" target="_blank" rel="noopener noreferrer">
        Privacy Policy
      </a>
    </label>
  </div>
)
