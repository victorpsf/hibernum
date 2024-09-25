import { Connector } from "../Connector"

export interface IHibernumClient {

}

export const HibernumClient = function (): IHibernumClient {
    const client = Connector({ href: process.env.REACT_APP_HIBERNUM_HREF || '' });

    return {

    }
}