import { ProductSummary } from "./ProductSummary.model";

export class ProductDetail extends ProductSummary {
    summary!: string;
    imageFile!: string;
    price!: number;
}