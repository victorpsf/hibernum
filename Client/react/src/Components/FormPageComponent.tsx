import React from "react";
import {IRoutePath} from "../Pages/Routes";
import {RouteContext} from "../lib/context/RouteContext";

export interface IRouteNavigation {
    path: IRoutePath;
    name: string;
}

export interface IFormPageComponentProps {
    title: string;
    navigation: IRouteNavigation[];
    content: JSX.Element;
    action: JSX.Element;
}

const FormPageComponent = function ({ title, navigation, content, action }: IFormPageComponentProps): JSX.Element {
    const routeContext = React.useContext(RouteContext);

    const isCurrentPath = (path: IRoutePath): boolean => path === routeContext.current();
    const getCurrentPathStyle = (path: IRoutePath): string => isCurrentPath(path) ? 'cursor-not-allowed text-black': 'text-blue-700 cursor-pointer hover:opacity-60';

    const NavigationElement = function ({}: {}): JSX.Element {
        const elements: JSX.Element[] = [];

        for (let x = 0; x < navigation.length; x++) {
            const route = navigation[x];

            if (x > 0)
                elements.push(
                    <p className={'mx-2'}>-</p>
                )

            elements.push(
                <p key={route.path} className={getCurrentPathStyle(route.path)} onClick={() => { if(!isCurrentPath(route.path)) routeContext.navigate(route.path) }}>{route.name}</p>
            )
        }

        return (
            <div className={'w-full p-2 flex flex-row'}>
                {elements}
            </div>
        )
    }

    return (
        <div className={'flex-1 m-2 bg-white rounded p-2'}>
            <div className={'flex-3 p-2 border-b-2 mb-4'}>
                <p className={'text-lg  font-bold'}>{title}</p>
                <NavigationElement />
            </div>
            <div>{content}</div>
            <div>{action}</div>
        </div>
    )
}

export default FormPageComponent;