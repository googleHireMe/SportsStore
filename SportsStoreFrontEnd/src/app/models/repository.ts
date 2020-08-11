import { Product } from './product.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Filter } from './configClasses.repository';
import { Supplier } from './supplier.model';

export const baseUrl = 'https://localhost:44397';

const productsUrlPart = '/api/products';
const productsUrl = `${baseUrl}${productsUrlPart}`;
const suppliersUrlPart = '/api/suppliers';
const suppliersUrl = `${baseUrl}${suppliersUrlPart}`;

@Injectable()
export class Repository {

  product: Product;
  products: Product[];
  suppliers: Supplier[] = [];

  get filter(): Filter {
    return this.filterObject;
  }

  private filterObject = new Filter();

  constructor(private http: HttpClient) {
    this.filter.related = true;
    this.getProducts();
  }

  getProduct(id: number) {
    this.http.get(`${productsUrl}/${id}`)
      .subscribe(response => {
        this.product = response;
      });
  }

  getProducts() {
    let params = new HttpParams()
      .set('related', `${this.filter.related ?? ''}`)
      .set('category', `${this.filter.category ?? ''}`)
      .set('search', `${this.filter.search ?? ''}`);

    this.http.get(`${productsUrl}`, { params })
      .subscribe(response => {
        this.products = response as Product[];
      });
  }

  getSuppliers() {
    this.http.get(`${suppliersUrl}`)
      .subscribe(response => {
        this.products = response as Supplier[];
      });
  }

  createProduct(prod: Product) {
    const data = {
      name: prod.name,
      category: prod.category,
      description: prod.description,
      price: prod.price,
      supplier: prod.supplier ? prod.supplier.supplierId : 0
    };

    this.http.post(productsUrl, data)
      .subscribe(response => {
        prod.productId = response as number;
        this.products.push(prod);
      });
  }

  createProductAndSupplier(prod: Product, supp: Supplier) {
    const data = {
      name: supp.name,
      city: supp.city,
      state: supp.state
    };
    this.http.post(suppliersUrl, data)
      .subscribe(response => {
        supp.supplierId = response as number;
        prod.supplier = supp;
        this.suppliers.push(supp);
        if (prod != null) {
          this.createProduct(prod);
        }
      });
  }

  replaceProduct(prod: Product) {
    let data = {
      name: prod.name,
      category: prod.category,
      description: prod.description,
      price: prod.price,
      supplier: prod.supplier ? prod.supplier.supplierId : 0
    };

    this.http.put(`${productsUrl}/${prod.productId}`, data)
      .subscribe(response => this.getProducts());
  }

  replaceSupplier(supp: Supplier) {
    let data = {
      name: supp.name,
      city: supp.city,
      state: supp.state
    };

    this.http.put(`${suppliersUrl}/${supp.supplierId}`, data)
      .subscribe(response => this.getProducts());
  }
}
