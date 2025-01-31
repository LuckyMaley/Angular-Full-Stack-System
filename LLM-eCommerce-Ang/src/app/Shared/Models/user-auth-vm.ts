export interface UserLoginFormVM {
  userName: string;
  password: string;
}

export interface UserRegistrationFormVM {
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
  password: string;
  address: string;
  phoneNumber: string;
  confirmPassword: string;
  userRole: string;
}

export interface UserRegToApiVM {
  username: string;
  email: string;
  firstname: string;
  lastname: string;
  password: string;
  address: string;
  phoneNumber: string;
  role: string;
}

export interface UserRegResponseVM {
  username: string;
}

export interface CurrentUserVM {
  userName: string;
  userRole: string;
}

export interface LoginResponseMessageVM {
  userName: string;
  userRole: string;
  firstName: string;
  lastName: string;
  fullName: string;
  isAdminUserRole: boolean;
  message: string;
}

export interface Roles {
    $id: string;
    $values: string[];
  }

export interface LoginResponseVM {
  token: string;
  expiration: string;
  firstName: string;
  lastName: string;
  userName: string;
  roles: Roles;
}

export interface CurrentLoggedInUserVM {
  token: string;
  expiration: string;
  firstName: string;
  lastName: string;
  userName: string;
  isadmin: boolean;
  userRole: string;
}

export interface UserProfileVM {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  address: string;
  phoneNumber: string;
  userRoles: Roles;
}

export interface UserProfileVMResponse {
  $id: string;
  $values: UserProfileVM[];
}

export interface UserAccountVM {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  address: string;
  phoneNumber: string;
  rolesHeld: Roles;
}

export interface UserAccountVMResponse {
  $id: string;
  $values: UserAccountVM[];
}

export interface UserAccountVMAdminSubmit {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  address: string;
  phoneNumber: string;
  role: string;
}

export interface UserAccountVMSubmit {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  address: string;
  phoneNumber: string;
}
