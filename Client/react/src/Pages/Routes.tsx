import React from 'react';
import { Route, Routes } from 'react-router-dom';
import { AuthContext } from '../lib/context/AuthContext';

import { IoMdHome } from "react-icons/io";
import { HomePage } from './home/Home';
import { ErrorPage } from './error/Error';
import { IoIosUnlock } from "react-icons/io";
import { RiBuilding2Fill } from "react-icons/ri";
import { LoginPage } from './login/Login';
import { CompanyPage } from './company/Company';

export type IRoutePath = '/' | '/login' | '/company' | '*';

export interface IRoute {
    path: IRoutePath;
    label: string;
    main?: boolean;
    logged?: boolean;
    view: boolean;
    icon?: JSX.Element;
    element: any;
}

export const routes: IRoute[] = [
    { path: '/', label: 'Página Inicial', icon: <IoMdHome size={25} />, main: true, element: HomePage, view: true },
    { path: '/login', label: 'Entrar', icon: <IoIosUnlock size={25} />, logged: false, element: LoginPage, view: true },
    { path: '/company', label: 'Empresa', icon: <RiBuilding2Fill size={25} />, logged: true, element: CompanyPage, view: true },
    { path: '*', label: 'Página de erro', element: ErrorPage, view: false }
]

export const AppRoutes = function (): JSX.Element {
    const { logged } = React.useContext(AuthContext);

    return (
        <Routes>
            {routes.filter(a => (a.logged === undefined || a.logged === logged)).map(a => <Route key={a.path} path={a.path} index={a.main} Component={a.element} />)}
        </Routes>
    )
}