import {Product} from "./Product";

export interface ProductType {
    id: number;
    value: string;
    product?: Product;
}