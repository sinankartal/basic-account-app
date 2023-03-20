export interface User {
  id: string;
  name: string;
  surname: string;
}


export interface Token {
  username:string;
  token:string;
}

export interface UserAccountsDTO {
  user: User;
  accounts: Account[];
}

export interface Transaction{
  accountId: string;
  accountNumber: string;
  amount: number;
  date: Date
}

export interface Account{
  id:string;
  accountNumber: string;
  balance: string;
  transactions: Transaction[];
}
