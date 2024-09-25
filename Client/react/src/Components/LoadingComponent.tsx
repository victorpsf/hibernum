import React from 'react'

export interface ILoading {
  value: boolean;
  text?: string;
}

export interface ILoadingComponentProps {
  size: 'small' | 'large' | 'tiny'
  loading?: ILoading;
  position: 'fixed' | 'static';
}

const LoadingSvg = function (props: ILoadingComponentProps): JSX.Element {
  if (props.size === 'large')
    return (
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        viewBox="0 0 100 100" 
        preserveAspectRatio="xMidYMid" 
        style={{ shapeRendering: 'auto', display: 'block', background: 'rgba(255, 255, 255, 0)' }} 
        width="200" 
        height="200" 
      >
        <g>
          <circle strokeDasharray="164.93361431346415 56.97787143782138" r="35" strokeWidth="10" stroke="#00a8e0" fill="none" cy="50" cx="50">
            <animateTransform keyTimes="0;1" values="0 50 50;360 50 50" dur="1s" repeatCount="indefinite" type="rotate" attributeName="transform"></animateTransform>
          </circle>
          <g></g>
        </g>
      </svg>
    )

  else if (props.size == 'small')
    return (
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        viewBox="0 0 100 100" 
        preserveAspectRatio="xMidYMid" 
        style={{ shapeRendering: 'auto', display: 'block', background: 'rgba(255, 255, 255, 0)' }} 
        width="50" 
        height="50" 
      >
        <g>
          <circle strokeDasharray="164.93361431346415 56.97787143782138" r="35" strokeWidth="10" stroke="#00a8e0" fill="none" cy="50" cx="50">
            <animateTransform keyTimes="0;1" values="0 50 50;360 50 50" dur="1s" repeatCount="indefinite" type="rotate" attributeName="transform"></animateTransform>
          </circle>
          <g></g>
        </g>
      </svg>
    );

  else 
    return (
      <svg 
        xmlns="http://www.w3.org/2000/svg" 
        viewBox="0 0 100 100" 
        preserveAspectRatio="xMidYMid" 
        style={{ shapeRendering: 'auto', display: 'block', background: 'rgba(255, 255, 255, 0)' }} 
        width="15" 
        height="15" 
      >
        <g>
          <circle strokeDasharray="164.93361431346415 56.97787143782138" r="35" strokeWidth="10" stroke="#00a8e0" fill="none" cy="50" cx="50">
            <animateTransform keyTimes="0;1" values="0 50 50;360 50 50" dur="1s" repeatCount="indefinite" type="rotate" attributeName="transform"></animateTransform>
          </circle>
          <g></g>
        </g>
      </svg>
    );
}

const LoadingStaticComponent = function (props: ILoadingComponentProps): JSX.Element {
  return (
    <div className='w-full flex flex-col justify-center items-center'>
      <LoadingSvg {...props} />
      {props.loading?.text && (<p className={props.position === 'static' ? 'opacity-60 mt-3': 'mt-3 text-white'}>{props.loading?.text}</p>)}
    </div>
  );
}

const LoadingComponent = function (props: ILoadingComponentProps): JSX.Element {

  if (props.position === 'static')
    return (<LoadingStaticComponent {...props} />)

  else 
    return (
      <div className='fixed flex justify-center items-center w-full h-full' style={{ backgroundColor: 'rgba(0,0,0,0.8)' }}>
        <LoadingStaticComponent {...props} />
      </div>
    )
}

export default LoadingComponent;