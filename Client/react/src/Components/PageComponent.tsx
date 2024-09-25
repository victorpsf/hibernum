
import React from "react";
import { IoMenu } from "react-icons/io5";
import { IRoute, routes } from "../Pages/Routes";
import { AuthContext } from "../lib/context/AuthContext";
import { ActionComponent } from "./ActionComponent";
import { RouteContext } from "../lib/context/RouteContext";

export interface IPageComponentProps {
    children: JSX.Element;
}

export const PageComponent = function ({ children }: IPageComponentProps): JSX.Element {
    const [open, setOpen] = React.useState<boolean>(false); 
    const { logged, signOut } = React.useContext(AuthContext);
    const RouteCtx = React.useContext(RouteContext);

    const onRouteClick = function (route: IRoute): void {
        if (route.path === RouteCtx.current())
            return;

        setOpen(false);
        RouteCtx.navigate(route.path);
    }

    const Routes = function ({}: {}): JSX.Element {
        const routeList = routes.filter(a => a.view)
            .filter(a => a.logged === undefined || a.logged === logged);

        return (
            <div className="p-3">
                {routeList.map(a => (
                    <div key={a.path} className={`my-2 flex border-b p-2 ${RouteCtx.current() == a.path ? 'opacity-60 cursor-not-allowed': 'cursor-pointer hover:opacity-60'}`} onClick={() => onRouteClick(a)}>
                        <div className="mr-3">{a.icon}</div>
                        <div>{a.label}</div>
                    </div>
                ))}
            </div>
        )
    }

    return (
        <div className="h-full bg-gray-800 text-gray-800">
            <div className="w-full h-[40px] p-2 bg-white flex justify-between items-center">
                <div className="cursor-pointer hover:opacity-60" onClick={() => setOpen(!open)}>
                    <IoMenu size={25} />
                </div>

                {!logged && (<ActionComponent text="sign-in" onPress={() => {
                    RouteCtx.navigate('/login');
                }} />)}
                {logged && (<ActionComponent text="sign-out" onPress={() => {
                    signOut();
                    RouteCtx.navigate('/');
                }} />)}
            </div>

            <div className="w-full h-[calc(100%-40px)] flex">
                {open && (
                    <div className="w-[300px] border-t shadow-ms shadow-gray-200 bg-white h-full">
                        <Routes />
                    </div>
                )}

                <div className={`${open ? 'w-[calc(100%-300px)]': 'w-full'} h-full p-2`}>{children}</div>
            </div>
        </div>
    )
}