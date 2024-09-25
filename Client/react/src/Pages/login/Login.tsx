import React from "react";
import TextField from "../../Components/fields/TextField"
import { PageComponent } from "../../Components/PageComponent"
import { ActionComponent } from "../../Components/ActionComponent";
import { AuthContext } from "../../lib/context/AuthContext";
import { DispatchContext } from "../../lib/context/DispatchContext";
import { IJSON } from "../../lib/db/Connector";
import { RouteContext } from "../../lib/context/RouteContext";
import PasswordField from "../../Components/fields/PasswordField";

export const LoginPage = function (): JSX.Element {
    const { signIn } = React.useContext(AuthContext);
    const { auth } = React.useContext(DispatchContext)
    const route = React.useContext(RouteContext);

    const [name, setName] = React.useState<string | undefined>(undefined);
    const [email, setEmail] = React.useState<string | undefined>(undefined);
    const [password, setPassword] = React.useState<string | undefined>(undefined);
    const [loading, setLoading] = React.useState<boolean>(false);
    const [errors, setErros] = React.useState<string[]>([]);

    const logar = async (): Promise<void> => {
        try {
            setLoading(true);
            var res = await auth.Authentication({
                name,
                email,
                password
            })
            setLoading(false);

            if (res.result.failed) {
                const err: string[] = [];
                for (const key of Object.keys(res.result.data as IJSON))
                    err.push((res.result.data as IJSON)[key]);
                setErros(err);
                return;
            }

            if (!res.result.failed && res.result.data?.token && res.result.data?.type) {
                signIn(res.result.data.token, res.result.data.type);
                route.navigate('/');
                return;
            }
        }

        catch (ex) 
        { }

        finally 
        { setLoading(false); }
    }

    return (
        <PageComponent>
            <div className="w-full h-full flex justify-center items-center">
                <div className="bg-white rounded shadow-ms shadow-gray-200 p-4 w-[60vw]">
                    <div className="mb-2">
                        <div className="py-1">
                            <TextField label="Nome" value={name} onTextChange={(value) => setName(value)} />
                        </div>
                        <div className="py-1">
                            <TextField label="E-mail" value={email} onTextChange={(value) => setEmail(value)} />
                        </div>
                        <div className="py-1">
                            <PasswordField label="Senha" value={password} onTextChange={(value) => setPassword(value)} />
                        </div>
                    </div>

                    {errors.length > 0 && (
                        <div className="p-2 list-disc">
                            {errors.map((a, i) => <div key={`${i}-${a}`} className="text-red-400">{a}</div>)}
                        </div>
                    )}

                    <div className="flex justify-center items-center">
                        <ActionComponent 
                            text="Logar" 
                            // disabled={((name.length === 0 && email.length === 0) || password.length < 8)} 
                            loading={loading}
                            onPress={logar} 
                        />
                    </div>
                </div>
            </div>
        </PageComponent>
    )
}