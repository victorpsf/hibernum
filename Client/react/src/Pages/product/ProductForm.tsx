import React from "react";
import {PageComponent} from "../../Components/PageComponent";
import {RouteContext} from "../../lib/context/RouteContext";
import {DispatchContext} from "../../lib/context/DispatchContext";
import LoadingComponent from "../../Components/LoadingComponent";
import {GetProductsParams} from "../../lib/db/client/params/GetProductsParams";
import TextField from "../../Components/fields/TextField";
import {ActionComponent} from "../../Components/ActionComponent";
import SelectField from "../../Components/fields/SelectField";
import {ProductGroup} from "../../lib/models/ProductGroup";
import FormPageComponent from "../../Components/FormPageComponent";
import FileField from "../../Components/fields/FileField";
import {FileReaderDto, toFileDto} from "../../lib/models/FileReaderDto";
import {FileEntity} from "../../lib/models/File";

const ProductForm = function (): JSX.Element {
    const route = React.useContext(RouteContext);
    const dispatch = React.useContext(DispatchContext);

    const [productGroup, setProductGroup] = React.useState<ProductGroup[]>([])

    const [id, setId] = React.useState<number | undefined>();
    const [name, setName] = React.useState<string>();
    const [size, setSize] = React.useState<string>();

    const [file, setFile] = React.useState<FileEntity | undefined>(undefined);
    const [fileDto, setFileDto] = React.useState<FileReaderDto | undefined>(undefined);
    const [group, setGroup] = React.useState<ProductGroup | undefined>();
    const [loading, setLoading] = React.useState(false);

    const init = async function (): Promise<void> {
        const query = route.query<GetProductsParams>();

        try {
            const { result: { data } } = await dispatch.hibernum.GetProductGroup({});
            setProductGroup((data || []).map(a => { a.products = []; return a; }));
        }

        catch (error) { }
        finally { }

        if (query.id)
            try {
                setLoading(true);
                const { result: { data } } = await dispatch.hibernum.GetProducts({
                    id: query.id
                })

                if (!data || (data?.length || 0) === 0) {
                    route.navigate('*', { error: 404 });
                    return;
                }

                setId(data[0].id);
                setName(data[0].name);
                setSize(data[0].size);
                setFile(data[0].file as FileEntity);
                setFileDto(toFileDto(data[0].file));
                setGroup(data[0].group);
            }

            catch (error) { }

            finally
            { setLoading(false); }
    }

    const SaveProduct = async function (): Promise<void> {

        if (!fileDto && file)
            await dispatch.hibernum.RemoveFile({ id: file.id });

        else if (file?.id && fileDto?.id !== file?.id)
            await dispatch.hibernum.RemoveFile({ id: file.id })

        let fileId: number | null = null;
        if (fileDto) {
            var result = await dispatch.hibernum.SaveFile(fileDto);
            fileId = result.result.data?.id || null;
        }

        const resultProduct = await dispatch.hibernum.SaveProduct({
            id: id,
            name: name,
            size: size,
            file: fileId,
            group: group?.id
        })

        if (resultProduct.result.data)
            route.goBack();
    }

    React.useEffect(() => {
        init()
    }, []);

    return (
        <PageComponent>
            <div className={'w-full'}>
                {loading && (
                    <div className={'w-full h-full'}>
                        <div className={'p-6 bg-white rounded shadow shadow-white'}>
                            <LoadingComponent loading={{value: true}} size={'small'} position={'static'}/>
                        </div>
                    </div>
                )}

                <FormPageComponent
                    title={'Formulario Produto'}
                    navigation={[{
                        name: 'Página Inicial',
                        path: '/'
                    }, {
                        name: 'Produtos',
                        path: '/product'
                    }, {
                        name: 'Formulario Produto',
                        path: '/product/form'
                    }]}
                    content={
                        <div>
                            <div className={'my-2'}>
                                <TextField
                                    label={'Nome'}
                                    value={name}
                                    onTextChange={setName}
                                />
                            </div>
                            <div className={'my-2'}>
                                <TextField
                                    label={'Tamanho'}
                                    value={size}
                                    onTextChange={setSize}
                                />
                            </div>
                            <div className={'my-2'}>
                                <FileField
                                    label={'Imagem'}
                                    filter={'image/*'}
                                    value={fileDto ? [fileDto]: []}
                                    onFileChange={(files) => setFileDto(files[0])}
                                />
                            </div>
                            <div className={'my-2'}>
                                <SelectField
                                    label={'Grupo'}
                                    value={group}
                                    options={productGroup}
                                    handleSelectedElement={(value?: ProductGroup) => (
                                        <div className={'w-full'}>
                                            {!value && (
                                                <div></div>
                                            )}

                                            {value && (
                                                <div>{value.name}</div>
                                            )}
                                        </div>
                                    )}
                                    handleOptionElement={(option: ProductGroup) => (
                                        <div className={'my-1 p-2'}>
                                            {option.name}
                                        </div>
                                    )}
                                    onChange={(option?: ProductGroup) => setGroup(option)}
                                />
                            </div>
                        </div>
                    }
                    action={
                        <div className={'flex justify-end'}>
                            <ActionComponent
                                textColor={'blue'}
                                text={'Salvar'}
                                onPress={() => SaveProduct()}
                            />
                            <ActionComponent text={'Voltar'} onPress={route.goBack}/>
                        </div>
                    }
                />
            </div>

        </PageComponent>
    )
}

export default ProductForm