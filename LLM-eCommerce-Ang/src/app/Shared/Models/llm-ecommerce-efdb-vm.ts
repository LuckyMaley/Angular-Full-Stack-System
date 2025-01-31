import { UserProfileVM } from "./user-auth-vm";

export interface CartItem {
  id: string;
  title: string;
  image: string;
  price: number;
  qty: number;
}

export interface Category {
  $id: string;
  categoryId: number;
  name: string;
  products: {
    $id: string;
    $values: Product[];
  };
}


export interface CategoriesResponse {
  $id: string;
  $values: Category[];
}

export interface CategoryVM {
  name: string;
}

export interface EfUser {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  efUserProducts: {
    $id: string;
    $values: EfUserProduct[];
  };
  orders: {
    $id: string;
    $values: Order[];
  };
  reviews: {
    $id: string;
    $values: Review[];
  };
  wishlists: {
    $id: string;
    $values: Wishlist[];
  };
}

export interface EfUserResponse {
  $id: string;
  $values: EfUser[];
}

export interface EfUserProduct {
  $id: string;
  efUserProductId: number;
  efUserId: number;
  productId: number;
  addedDate: string;
  efUser: EfUser;
  product: Product;
}

export interface EfUserProductResponse {
  $id: string;
  $values: EfUserProduct[];
}

export interface Order {
  $id: string;
  orderId: number;
  efUserId: number;
  shippingId: number;
  orderDate: string;
  totalAmount: number;
  efUser: EfUser;
  shipping: Shipping;
  orderDetails: {
    $id: string;
    $values: OrderDetail[];
  };
  payments: {
    $id: string;
    $values: Payment[];
  };
}

export interface OrderResponse {
  $id: string;
  $values: Order[];
}

export interface OrderDetail {
  $id: string;
  orderDetailId: number;
  orderId: number;
  productId: number;
  quantity: number;
  unitPrice: number;
  order: Order;
  product: Product;
}

export interface OrderDetailResponse {
  $id: string;
  $values: OrderDetail[];
}

export interface Payment {
  $id: string;
  paymentId: number;
  paymentMethod: string | null;
  paymentDate: string;
  orderId: number;
  amount: number;
  status: string | null;
  order: Order;
}

export interface PaymentResponse {
  $id: string;
  $values: Payment[];
}

export interface Product {
  $id: string;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  stockQuantity: number;
  modifiedDate: string;
  imageUrl: string;
  category: Category;
  efUserProducts: {
    $id: string;
    $values: EfUserProduct[];
  };
  orderDetails: {
    $id: string;
    $values: OrderDetail[];
  };
  reviews: {
    $id: string;
    $values: Review[];
  };
  wishlists: {
    $id: string;
    $values: Wishlist[];
  };
}

export interface ProductResponse {
  $id: string;
  $values: Product[];
}

export interface ProductsVM {
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  stockQuantity: number;
  imageUrl: string;
}

export interface Review {
  $id: String;
  reviewId: number;
  productId: number;
  efUserId: number;
  rating: number;
  title: string | null;
  comment: string | null;
  reviewDate: string;
  efUser: EfUser;
  product: Product;
}

export interface ReviewsVM {
  productId: number;
  rating: number;
  title: string | null;
  comment: string | null;
}

export interface ReviewResponse {
  $id: string;
  $values: Review[];
}

export interface Shipping {
  $id: string;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orders: {
    $id: string;
    $values: Order[];
  };
}

export interface ShippingResponse {
  $id: string;
  $values: Shipping[];
}

export interface ShippingsVM {
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
}

export interface Wishlist {
  $id: string;
  wishlistId: number;
  efUserId: number;
  productId: number;
  addedDate: string;
  efUser: EfUser;
  product: Product;
}

export interface WishlistsVM {
  productId: number;
}

export interface WishlistResponse {
  $id: string;
  $values: Wishlist[];
}

export interface ShippingsVM {
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
}

export interface PaymentsVM {
  paymentMethod: string | null;
  orderId: number;
  amount: number;
  status: string | null;
}

export interface OrderDetailsTwoVM {
  productId: number;
  quantity: number;
  unitPrice: number;
}

export interface FullOrderVM {
  shippingAddress?: string;
  shippingMethod?: string;
  totalAmount: number;
  paymentMethod?: string;
  status?: string;
  orderDetails: OrderDetailsTwoVM[];
}

export interface OrderResponseTwo {
  orderId: number;
}

export interface OrdersVM {
  shippingAddress: string | null;
  shippingMethod: string | null;
  totalAmount: number;
}

export interface OrderDetailsVM {
  orderId: number;
  productId: number;
  quantity: number;
  unitPrice: number;
}

export interface CustomerDashboardVM {
  userProfile: UserProfileVM;
  myOrdersVM: MyOrdersVM[];
}
export interface MyCustomersOrdersDetailsVMResponse {
  $id: string;
  customersOrders: CustomersOrderDetailsVMResponse;
}

export interface CustomersOrderDetailsVMResponse {
  $id: string;
  $values: CustomersOrderDetailsVM[];
}

export interface CustomersOrderDetailsVM {
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  orderId: number;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orderDate: string;
  totalAmount: number;
  orderDetailId: number;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  categoryName: string;
  stockQuantity: number;
  modifiedDate: string;
  quantity: number;
  unitPrice: number;
}

export interface CustomersOrderInfo {
  myOrdersVM: MyOrdersVM;
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
}

export interface CustomersOrdersVM {
  myOrdersVMList: MyOrdersVM[];
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
}

export interface MyCustomersOrdersVMResponse {
  $id: string;
  customersOrders: MyOrdersVMResponse;
}

export interface MyOrdersVMResponse {
  $id: string;
  $values: MyOrdersVM[];
}

export interface MyOrdersVM {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  orderId: number;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orderDate: string;
  totalAmount: number;
}

export interface CrudCategoriesProductsVM {
  categories: Category[];
  product: Product;
}

export interface OrderDetailsSellerVM {
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
  usersProductsOrdersVM: UsersProductsOrdersVM;
}

export interface OrderHistoryVM {
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
  usersProductsOrdersVMList: UsersProductsOrdersVM;
}

export interface SellerDashboardVM {
  userProfile: UserProfileVM;
  usersProductsOrders: UsersProductsOrdersVM[];
}

export interface UsersProductsVMObjectResponse {
  $id: string;
  usersProducts: UsersProductsVMResponse;
}

export interface UsersProductsVMResponse {
  $id: string;
  $values: UsersProductsVM[];
}

export interface UsersProductsVM {
  $id: string
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  categoryName: string;
  stockQuantity: number;
  modifiedDate: string;
  imageUrl: string | null;
}

export interface UsersProductsOrdersVMObjectResponse {
  $id: string;
  usersProductsOrders: UsersProductsOrdersVMResponse;
}

export interface UsersProductsOrdersVMResponse {
  $id: string;
  $values: MyOrdersVM[];
}

export interface UsersProductsOrdersVM {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  orderId: number;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orderDate: string;
  totalAmount: number;
}

export interface AdminCustomersOrdersVMResponse {
  $id: string;
  $values: AdminCustomersOrdersVM[];
}

export interface AdminCustomersOrdersVM {
  $id: string
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  orderId: number;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orderDate: string;
  totalAmount: number;
}

export interface AdminOrderHistoryVM {
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
  usersProductsOrdersVMList: AdminCustomersOrdersVM;
}

export interface AdminUpdateUserProfileVM {
  firstName: string | null;
  lastName: string | null;
  email: string | null;
  address: string | null;
  phoneNumber: string | null;
  userName: string;
  role: string | null;
}

export interface AdminUsersProductsVMResponse {
  $id: string;
  $values: AdminUserProductsVM[];
}

export interface AdminUserProductsVM {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  categoryName: string | null;
  stockQuantity: number;
  modifiedDate: string;
  imageUrl: string;
}

export interface DashboardVM {
  userProfile: UserProfileVM;
  customersOrders: AdminCustomersOrdersVM[];
}

export interface OrderDetailsAdminVM {
  customersOrderDetailsVMList: CustomersOrderDetailsVM[];
  usersProductsOrdersVM: AdminCustomersOrdersVM;
}

export interface CustomersReviewsVMObjectResponse {
  $id: string;
  customersReviews: CustomersReviewsVMResponse;
}

export interface CustomersReviewsVMResponse {
  $id: string;
  $values: CustomersReviewsVM[];
}

export interface CustomersReviewsVM {
  $id:string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  reviewId: number;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  categoryName: string;
  stockQuantity: number;
  modifiedDate: string;
  rating: number;
  title: string | null;
  comment: string | null;
  reviewDate: string;
}

export interface CustomersOrdersPaymentsVMResponse {
  $id: string;
  $values: CustomersOrdersPaymentsVM[];
}

export interface CustomersOrdersPaymentsVM {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  paymentId: number;
  paymentMethod: string | null;
  paymentDate: string;
  paymentAmount: number;
  paymentStatus: string | null;
  orderId: number;
  shippingId: number;
  shippingDate: string;
  shippingAddress: string | null;
  shippingMethod: string | null;
  trackingNumber: string | null;
  deliveryStatus: string | null;
  orderDate: string;
  totalAmount: number;
}

export interface CustomersWishlistsVMObjectResponse {
  $id: string;
  customersWishlists: CustomersWishlistsVMResponse;
}

export interface CustomersWishlistsVMResponse {
  $id: string;
  $values: CustomersWishlistsVM[];
}

export interface CustomersWishlistsVM {
  $id: string;
  efUserId: number;
  firstName: string;
  lastName: string;
  email: string;
  address: string | null;
  phoneNumber: string | null;
  identityUsername: string;
  role: string;
  wishlistId: number;
  productId: number;
  name: string | null;
  brand: string | null;
  description: string | null;
  type: string | null;
  price: number;
  categoryId: number;
  categoryName: string;
  stockQuantity: number;
  modifiedDate: string;
  addedDate: string;
}

