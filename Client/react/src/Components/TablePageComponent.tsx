import { FaEdit } from "react-icons/fa";
import {IRouteNavigation} from "./FormPageComponent";
import React from "react";
import {RouteContext} from "../lib/context/RouteContext";
import {IRoutePath} from "../Pages/Routes";

type IJSON = {
    [key: string]: IJSON | any;
}

interface HeaderProps {
    key: string;
    label: string;
}

type ITablePageComponentProps<T> = {
    title: string;
    navigation: IRouteNavigation[];
    searchElement: JSX.Element;
    headers: HeaderProps[];
    value: T[];
    onEdit: (value: T) => void;
}

const TablePageComponent = function <T>({ title, navigation, searchElement, headers, value, onEdit }: ITablePageComponentProps<T>): JSX.Element {
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

    const GetData = function (key: keyof T, data: T): any {
        if (data[key])
            return data[key];
        return '';
    }

    const TableElement = function ({}: {}): JSX.Element {
        return (
            <div className={'flex-1 p-2 rounded bg-white shadow shadow-white'}>
                <table className={'table-auto w-full'}>
                    <thead>
                        <tr>
                            {headers.map(({key, label}) => (<th>{label}</th>))}
                            <th></th>
                        </tr>
                    </thead>

                    <tbody>
                        {value.map((a) => (
                            <tr>
                                {headers.map(({key, label}) => (<td className={'text-center py-2'}>{GetData(key as keyof T, a)}</td>))}
                                <td>
                                    <div className={'cursor-pointer hover:opacity-60 py-2'} onClick={() => onEdit(a)}>
                                        <FaEdit/>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        );
    }

    return (
        <div className={'flex-1 m-2'}>
            <div className={'flex-3 p-2 bg-white rounded border-b-2 mb-4'}>
                <p className={'text-lg  font-bold'}>{title}</p>
                <NavigationElement />
            </div>

            <div className={'flex-2 p-2 mb-2 rounded bg-white shadow shadow-white'}>
                {searchElement}
            </div>

            {(value && value.length > 0) && (
                <div className={'flex-1 mt-4'}>
                    <TableElement />
                </div>
            )}
        </div>
    )
}

export default TablePageComponent