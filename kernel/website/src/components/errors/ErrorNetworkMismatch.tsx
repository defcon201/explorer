import * as React from 'react'

import './Errors.css'

export interface ErrorNetworkMismatchProps {
  details: { tld: string; web3Net: string; tldNet: string } | null
}
export const ErrorNetworkMismatch: React.FC<ErrorNetworkMismatchProps> = (props) => (
  <div id="error-networkmismatch" className="error-container">
    <div className="error-background" />
    <div className="errormessage">
      <div className="errortext col">
        <div className="communicationslink">A network mismatch was detected</div>
        <div className="givesomedetailof">
          We detected that you are entering the <strong id="tld">{props.tld || 'zone'}</strong> domain with your
          Ethereum wallet set to <strong id="web3Net">{props.web3Net || 'mainnet'}</strong>.
        </div>
        <div className="givesomedetailof">
          To continue, please change the Ethereum network in your wallet to{' '}
          <strong id="web3NetGoal">{props.tldNet || 'ropsten'}</strong> and click "Reload".
        </div>
        <div className="cta">
          <button
            className="retry"
            onClick={() => {
              window.location.reload()
            }}
          >
            Reload
          </button>
        </div>
      </div>
      <div className="errorimage col">
        <div className="imagewrapper">
          <img alt="" className="error-image" src="/images/robots/robotsmiling.png" />
        </div>
      </div>
    </div>
  </div>
)
