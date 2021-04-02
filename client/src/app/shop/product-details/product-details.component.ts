import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/share/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  constructor(private shopService: ShopService, private activateRoute: ActivatedRoute) { }
  productDetail: IProduct;

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
  this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.productDetail = product;
    }, error=> {
      console.log(error);      
    });
  }

}
