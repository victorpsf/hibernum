import React from 'react';
import ReactDOM from 'react-dom/client';

import reportWebVitals from './reportWebVitals';

import './assets/index.css';
import { AuthProvider } from './lib/context/AuthContext';
import { BrowserRouter } from 'react-router-dom';
import { AppRoutes } from './Pages/Routes';
import { RouteProvider } from './lib/context/RouteContext';
import { DispatchProvider } from './lib/context/DispatchContext';


ReactDOM.createRoot(document.getElementById('root') as HTMLElement)
  .render(
    <React.StrictMode>
      <BrowserRouter>
        <RouteProvider>
          <AuthProvider>
            <DispatchProvider>
              <AppRoutes />
            </DispatchProvider>
          </AuthProvider>
        </RouteProvider>
      </BrowserRouter>
    </React.StrictMode>
  );


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
