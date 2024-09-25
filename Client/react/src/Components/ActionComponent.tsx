import LoadingComponent from "./LoadingComponent";

export type IActionTextColor = 'white' | 'black' | 'red' | 'blue' | 'yellow' | 'green'
export type IActionBorderColor = 'white' | 'black' | 'red' | 'blue' | 'yellow' | 'green'
export type IActionBgColor = 'white' | 'black' | 'red' | 'blue' | 'yellow' | 'green'

export interface IActionProps {
    key?: string;
    text: string;
    textColor?: IActionTextColor; // default white
    border?: boolean;
    borderColor?: IActionBorderColor; // default blue
    bgColor?: IActionBgColor;
    appendStyle?: string;
    paddingX?: string;
    paddingY?: string;

    disabled?: boolean;
    loading?: boolean;
    className?: string;
    textClassName?: string;
    icon?: JSX.Element;

    onPress: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

export const ActionComponent = function (props: IActionProps): JSX.Element {
    const getTextClassName = function () {
        switch (props.textColor) {
            case "white":   return "text-white";
            case "blue":    return "text-blue-500";
            case "red":     return "text-red-600";
            case "yellow":  return "text-yellow-500";
            case "green":   return "text-green-500";
            case "black":
            default:        return "text-black";
        }
    }

    const getBgClassName = function (): string {
        switch (props.bgColor) {
            case "black":   return "bg-black";
            case "red":     return "bg-red-600";
            case "yellow":  return "bg-yellow-500"
            case "green":   return "bg-green-500";
            case "blue":    return "bg-blue-600";
            case "white":
            default:        return "bg-white";
        }
    }

    const getBorderColor = function (): string {
        switch (props.borderColor) {
            case 'red':     return 'border-red-600';
            case 'white':   return 'border-white';
            case "yellow":  return "border-yellow-500"
            case "green":   return "border-green-500";
            case 'blue':    return 'border-blue-600';
            case 'black':
            default:        return 'border-black';
        }
    }

    const getClassName = function (): string {
        const classNameList: string[] = [
            'rounded', 
            props.disabled ? '': 'hover:opacity-80',
            props.paddingX ?? 'px-3', 
            props.paddingY ?? 'py-2',
            props.border ? 'border-2': '',
            props.disabled || props.loading ? 'opacity-60 cursor-not-allowed': '',
            props.className ?? ''
        ];

        classNameList.push(getTextClassName());
        classNameList.push(getBgClassName());
        if (props.border) classNameList.push(getBorderColor());

        return classNameList.join(' ');
    }

    return (
        <button
            key={props.key}
            disabled={props.disabled || props.loading}
            className={getClassName()}
            onClick={(event: React.MouseEvent<HTMLButtonElement>) => props.onPress(event)}
        >
            {
                props.loading && (
                    <div className='min-w-[6vw]'>
                        <LoadingComponent
                            size='tiny'
                            loading={{
                                value: props.loading,
                                text: undefined
                            }}
                            position='static'
                        />
                    </div>
                )
            }
            
            {!props.loading && (
                <div className={`h-[100%] ${props.icon ? 'flex items-center': ''}`}>
                    <div className={props.icon ? `${props.textClassName || ''} w-[calc(100%-20px)]`: `${props.textClassName || ''} w-full`}>{props.text}</div>
                    {props.icon && (<div className='w-[20px]'>{props.icon}</div>)}
                </div>
            )}
        </button>
    )
}