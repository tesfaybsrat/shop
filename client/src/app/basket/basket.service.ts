import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../share/models/basket';
import { IProduct } from '../share/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
baseUrl = environment.apiUrl;
private basketSource = new BehaviorSubject<IBasket>(null);
basket$ = this.basketSource.asObservable();
private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http : HttpClient) { }

  getBasket(id: string){
     return this.http.get(this.baseUrl + "basket?id=" +id)
          .pipe(
          map((basket: IBasket) =>{
            this.basketSource.next(basket);
            this.calculateTotals();
            //console.log(this.getCurrentBasketValue());
          })
          );
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl+ 'basket',basket).subscribe((response: IBasket) =>{
      this.basketSource.next(response);
      this.calculateTotals(); 
      //console.log(response);
    },error=>{
      console.log(error);
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductToBasketItem(item, quantity);
    let basket = this.getCurrentBasketValue();
    if(basket === null){
      basket = this.createBasket();
    } 
    //console.log(basket);
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuality(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuality(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else{
      this.removeItemFromBasket(item);
    }   
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x=>x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id);
      if(basket.items.length > 0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket);
      }
    }
  }
  deleteBasket(basket: IBasket){
    return this.http.delete(this.baseUrl + 'basket?id=' +basket.id).subscribe(()=>{
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error=>{
      console.log(error);
    });
  }
  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    //b = represents item and each item has aprice and quantity then multiply to gether
    //a = store or add to a. NB a initial value is 0
    const subtotal = basket.items.reduce((a,b)=> (b.price * b.quantity) + a,0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, total, subtotal});
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    //console.log(items);
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if(index === -1){
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else{
      items[index].quantity +=quantity;
    }
    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }
  mapProductToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand:item.productBrand,
      type: item.productType
    };
  }
}
