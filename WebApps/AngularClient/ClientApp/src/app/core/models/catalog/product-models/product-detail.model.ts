import {ProductSummary} from "./product-summary.model";

export class ProductDetail extends ProductSummary {
  summary: string;
  imageFile: string;
  price: number;

  constructor() {
    super();
    this.summary = "";
    this.imageFile = "";
    this.price = 0;
  }
}
