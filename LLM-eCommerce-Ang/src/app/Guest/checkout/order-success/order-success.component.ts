import { Component, OnInit } from '@angular/core';
import { Order, Payment } from 'src/app/Shared/Models/llm-ecommerce-efdb-vm';
import { Shipping } from '../../../Shared/Models/llm-ecommerce-efdb-vm';
import { ActivatedRoute } from '@angular/router';
import { CrudService } from 'src/app/Shared/Services/crud.service';

@Component({
  selector: 'app-order-success',
  templateUrl: './order-success.component.html',
  styleUrls: ['./order-success.component.css'],
})
export class OrderSuccessComponent implements OnInit{
  orders: Order[] = [];
  order: Order = {
    $id: '155',
    orderId: 0,
    efUserId: 0,
    shippingId: 0,
    orderDate: new Date().toISOString(),
    totalAmount: 0,
    efUser: {
      $id: '',
      efUserId: 66,
      firstName: '',
      lastName: '',
      email: '',
      address: '',
      phoneNumber: '',
      identityUsername: '',
      role: '',
      efUserProducts: {
        $id: '',
        $values: [],
      },
      orders: {
        $id: '',
        $values: [],
      },
      reviews: {
        $id: '',
        $values: [],
      },
      wishlists: {
        $id: '',
        $values: [],
      },
    },
    shipping: {
      $id: '1',
      shippingId: 0,
      shippingAddress: '',
      shippingMethod: '',
      shippingDate: new Date().toISOString(),
      deliveryStatus: 'Pending',
      trackingNumber: '',
      orders: {
        $id: '',
        $values: [],
      },
    },
    orderDetails: {
      $id: '1',
      $values: [],
    },
    payments: {
      $id: '1',
      $values: [],
    }
  }
  shippings: Shipping[] = [];
  shipping: Shipping =  {
    $id: '1',
    shippingId: 0,
    shippingAddress: '',
    shippingMethod: '',
    shippingDate: new Date().toISOString(),
    deliveryStatus: 'Pending',
    trackingNumber: '',
    orders: {
      $id: '',
      $values: [],
    },
  };
  payments: Payment[] = [];
  payment: Payment = {
    $id: '1',
    paymentId: 0,
    orderId: 0,
    paymentMethod: '',
    status: '',
    amount: 0,
    paymentDate: new Date().toISOString(),
    order:{
      $id: '155',
      orderId: 0,
      efUserId: 0,
      shippingId: 0,
      orderDate: new Date().toISOString(),
      totalAmount: 0,
      efUser: {
        $id: '',
        efUserId: 66,
        firstName: '',
        lastName: '',
        email: '',
        address: '',
        phoneNumber: '',
        identityUsername: '',
        role: '',
        efUserProducts: {
          $id: '',
          $values: [],
        },
        orders: {
          $id: '',
          $values: [],
        },
        reviews: {
          $id: '',
          $values: [],
        },
        wishlists: {
          $id: '',
          $values: [],
        },
      },
      shipping: {
        $id: '1',
        shippingId: 0,
        shippingAddress: '',
        shippingMethod: '',
        shippingDate: new Date().toISOString(),
        deliveryStatus: 'Pending',
        trackingNumber: '',
        orders: {
          $id: '',
          $values: [],
        },
      },
      orderDetails: {
        $id: '1',
        $values: [],
      },
      payments: {
        $id: '1',
        $values: [],
      }
    }
  };
  errorMessage: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private crudService: CrudService
  ) {}

  ngOnInit(): void {
    this.fetchData();
    this.route.paramMap.subscribe((paramMap) => {
      let stringId: any = paramMap.get('id');
      let id = Number.parseInt(stringId);
      this.fetchDataWithId(id);
      this.getOrder(id);
      
    });
  }

  fetchData(): void {
    this.crudService.getOrders().subscribe(
      (data: Order[]) => {
        this.orders = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching orders';
        console.error(error);
      }
    );
    this.crudService.getShippings().subscribe(
      (data: Shipping[]) => {
        this.shippings = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching shippings';
        console.error(error);
      }
    );
    this.crudService.getPayments().subscribe(
      (data: Payment[]) => {
        this.payments = data;
      },
      (error) => {
        this.errorMessage = 'Error fetching payments';
        console.error(error);
      }
    );
  }

  fetchDataWithId(id: number): void {
    this.crudService.getOrderById(id).subscribe(
      (data: Order) => {
        this.order = data;
        this.payment = this.getPayment(data.orderId);
        this.shipping = this.getShipping(data.shippingId);
      },
      (error) => {
        this.errorMessage = 'Error fetching order';
        console.error(error);
      }
    );
  }

  getOrder(id: number): Order {
    const order = this.orders.find((p) => p.orderId === id);
    if (!order) {
      const defaultOrder: Order = {
        $id: '155',
        orderId: 0,
        efUserId: 0,
        shippingId: 0,
        orderDate: new Date().toISOString(),
        totalAmount: 0,
        efUser: {
          $id: '',
          efUserId: 66,
          firstName: '',
          lastName: '',
          email: '',
          address: '',
          phoneNumber: '',
          identityUsername: '',
          role: '',
          efUserProducts: {
            $id: '',
            $values: [],
          },
          orders: {
            $id: '',
            $values: [],
          },
          reviews: {
            $id: '',
            $values: [],
          },
          wishlists: {
            $id: '',
            $values: [],
          },
        },
        shipping: {
          $id: '1',
          shippingId: 0,
          shippingAddress: '',
          shippingMethod: '',
          shippingDate: new Date().toISOString(),
          deliveryStatus: 'Pending',
          trackingNumber: '',
          orders: {
            $id: '',
            $values: [],
          },
        },
        orderDetails: {
          $id: '1',
          $values: [],
        },
        payments: {
          $id: '1',
          $values: [],
        },
      };
      return defaultOrder;
    }
    return order;
  }

  getShipping(id: number): Shipping {
    const shipping = this.shippings.find((s) => s.shippingId === id);
    if (!shipping) {
      const defaultShipping: Shipping = {
        $id: '1',
        shippingId: 0,
        shippingAddress: '',
        shippingMethod: '',
        shippingDate: new Date().toISOString(),
        deliveryStatus: 'Pending',
        trackingNumber: '',
        orders: {
          $id: '',
          $values: [],
        },
      };
      return defaultShipping;
    }
    return shipping;
  }

  getPayment(orderId: number): Payment {
    const payment = this.payments.find((p) => p.orderId === orderId);
    if (!payment) {
      const defaultPayment: Payment = {
        $id: '1',
        paymentId: 0,
        orderId: orderId,
        paymentMethod: '',
        status: '',
        amount: 0,
        paymentDate: new Date().toISOString(),
        order:{
          $id: '155',
          orderId: 0,
          efUserId: 0,
          shippingId: 0,
          orderDate: new Date().toISOString(),
          totalAmount: 0,
          efUser: {
            $id: '',
            efUserId: 66,
            firstName: '',
            lastName: '',
            email: '',
            address: '',
            phoneNumber: '',
            identityUsername: '',
            role: '',
            efUserProducts: {
              $id: '',
              $values: [],
            },
            orders: {
              $id: '',
              $values: [],
            },
            reviews: {
              $id: '',
              $values: [],
            },
            wishlists: {
              $id: '',
              $values: [],
            },
          },
          shipping: {
            $id: '1',
            shippingId: 0,
            shippingAddress: '',
            shippingMethod: '',
            shippingDate: new Date().toISOString(),
            deliveryStatus: 'Pending',
            trackingNumber: '',
            orders: {
              $id: '',
              $values: [],
            },
          },
          orderDetails: {
            $id: '1',
            $values: [],
          },
          payments: {
            $id: '1',
            $values: [],
          }
        }
      };
      return defaultPayment;
    }
    return payment;
  }
}

