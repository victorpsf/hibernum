import {Product} from "./Product";

export interface ProductGroup {
    id: number;
    name: string;
    products?: Product[];
}