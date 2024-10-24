import React, {isValidElement} from "react";
import { PageComponent } from "../../Components/PageComponent";
import {DispatchContext} from "../../lib/context/DispatchContext";
import {Product} from "../../lib/models/Product";
import {IBaseConnectorResult} from "../../lib/db/Connector";
import {GetProductsParams} from "../../lib/db/client/params/GetProductsParams";
import TablePageComponent from "../../Components/TablePageComponent";
import NumericField from "../../Components/fields/NumericField";
import TextField from "../../Components/fields/TextField";
import {ActionComponent} from "../../Components/ActionComponent";
import {RouteContext} from "../../lib/context/RouteContext";

const ProductPage = function (): JSX.Element {
    const [loading, setLoading] = React.useState<boolean>(false);
    const [codigo, setCodigo] = React.useState<string | undefined>();
    const [name, setName] = React.useState<string | undefined>();
    const [tamanho, setTamanho] = React.useState<string | undefined>();
    const [produts, setProducts] = React.useState<Product[]>([]);

    const router = React.useContext(RouteContext);
    const dispatch = React.useContext(DispatchContext);

    const handleResponse = function ({ result: { data } }: IBaseConnectorResult<GetProductsParams, any, Product[]>): void {
        console.log(produts)
        setProducts(data ?? []);
    }

    const handleSearch = (): void => {
        setLoading(true);
        dispatch.hibernum.GetProducts({
            id: (codigo ? parseInt(codigo) : undefined),
            name,
            size: tamanho,
        })
            .then(handleResponse)
            .finally(() => setLoading(false));
    }

    const FormSearch = function ({}: {}): JSX.Element {
        return (
            <div className={'w-full'}>
                <div className={'mb-2'}>
                    <NumericField label={'Código'} value={codigo} onTextChange={(value?: string) => setCodigo(value)}/>
                </div>

                <div className={'mb-2'}>
                    <TextField label={'Nome'} value={name} onTextChange={(value?: string) => setName(value)}/>
                </div>

                <div className={'mb-2'}>
                    <TextField label={'Tamanho'} value={tamanho} onTextChange={(value?: string) => setTamanho(value)}/>
                </div>

                <div className={'flex justify-between'}>
                    <div className={'flex'}>
                        <ActionComponent
                            textColor={'blue'}
                            text={'Novo'}
                            loading={loading}
                            disabled={loading}
                            onPress={() => router.navigate('/product/form')}
                        />
                    </div>
                    <div className={'flex justify-end'}>
                        <div className={'mx-1'}>
                            <ActionComponent
                                textColor={'blue'}
                                text={'Pesquisar'}
                                loading={loading}
                                disabled={loading}
                                onPress={() => handleSearch()}
                            />
                        </div>

                        <div className={'mx-1'}>
                            <ActionComponent
                                textColor={'red'}
                                text={'Limpar'}
                                onPress={() => {
                                    setProducts([]);

                                    setCodigo(undefined)
                                    setName(undefined)
                                    setTamanho(undefined)
                                }}
                            />
                        </div>

                        <div className={'mx-1'}>
                            <ActionComponent
                                text={'Voltar'}
                                onPress={() => router.goBack()}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    return (
        <PageComponent>
            <div className={'w-full h-full'}>
                <TablePageComponent
                    title={'Pesquisar Produto'}
                    navigation={[{
                        name: 'Página Inicial',
                        path: '/'
                    }, {
                        name: 'Produtos',
                        path: '/product'
                    }]}
                    searchElement={<FormSearch/>}
                    headers={[
                        { key: 'id', label: 'Código' },
                        { key: 'name', label: 'Nome' },
                        { key: 'size', label: 'Tamanho' }
                    ]}
                    value={produts}
                    onEdit={(data: Product) => router.navigate('/product/form', { id: data.id })}
                />
            </div>
        </PageComponent>
    )
}

export default ProductPage;