import React from "react";
import { PageComponent } from "../../Components/PageComponent"
import { Company } from "../../lib/models/Company";
import LoadingComponent from "../../Components/LoadingComponent";
import { AuthContext } from "../../lib/context/AuthContext";
import { DispatchContext } from "../../lib/context/DispatchContext";
import { RouteContext } from "../../lib/context/RouteContext";
import { IBaseConnectorResult } from "../../lib/db/Connector";
import { ActionComponent } from "../../Components/ActionComponent";
import { AiOutlineSelect } from "react-icons/ai";

export const CompanyPage = function (): JSX.Element {
    const { user, signIn } = React.useContext(AuthContext);
    const { navigate } = React.useContext(RouteContext);
    const { auth } = React.useContext(DispatchContext);

    const [loading, setLoading] = React.useState<boolean>(true);
    const [companies, setCompanies] = React.useState<Company[]>([]);

    const onAuthenticationCompany = function (response: IBaseConnectorResult<any, any, Company[]>) {
        setCompanies(response.result.data || []);
        setLoading(false);
    }

    const onCompanySelect = async function (value: Company): Promise<void> {
        if (user?.companyId === value.id) {
            navigate('/');
            return;
        }

        try {
            setLoading(true);

            const response = await auth.AuthenticationCompany({ id: value.id });
            if (response.result.data?.token && response.result.data?.type) {
                signIn(response.result.data?.token, response.result.data?.type);
                navigate('/');
            }
        }

        catch (ex) 
        {}

        finally 
        { setLoading(false); }
    }

    React.useEffect(() => {
        auth.AuthenticationCompanies().then(onAuthenticationCompany)
    }, []);

    return (
        <PageComponent>
            <div className="w-full h-full flex justify-center items-center">
                <div className="p-4 bg-white rounded shadow-ms shadow-gray-200">
                    {loading && <LoadingComponent size='small' position='static' />}

                    {!loading && companies.length == 0 && (
                        <div className="flex flex-col justify-center items-center">
                            <p className="p-2 text-gray-800">Não existe empresa vinculada a você</p>
                            <ActionComponent text="Voltar" onPress={() => navigate('/')} />
                        </div>
                    )}

                    {!loading && companies.length > 0 && (
                        <div className="w-full min-w-[40vw]">
                            <p className="font-bold text-lg pb-4 text-gray-600">Empresa(s) que você tem vinculo</p>

                            {companies.map(a => (
                                <div key={a.id} className="p-4 w-full flex justify-between items-center rounded border border-black cursor-pointer hover:opacity-60" onClick={() => onCompanySelect(a)}>
                                    <div>
                                        <span className="font-bold mr-2">Empresa:</span>
                                        <span>{a.name}</span>
                                    </div>
                                    <div className="ml-4">
                                        <AiOutlineSelect size={25} />
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </PageComponent>
    )
}