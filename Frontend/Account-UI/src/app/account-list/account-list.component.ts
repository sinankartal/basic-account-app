import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UserService } from 'src/service/user-service';
import { Account, Transaction, User, UserAccountsDTO } from 'src/model/models';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'src/service/account-service';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss'],
})
export class AccountListComponent implements OnInit {
  userAccountInfo: UserAccountsDTO | undefined;
  userId!: string;
  users: User[] | undefined;
  selecteduser: User | undefined;
  createNewAccountSelected = false;
  newAccountInitialAmount = 0;
  showDialog = false;
  selectedAccountNumber = '';
  selectedAccountTransaction: Transaction[] | undefined;
  loading = false;
  addTransactionSelected = false;
  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef,
    private activeRoute: ActivatedRoute,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.activeRoute.params.subscribe((params) => {
      this.userId = params['id'];
      this.getUsers();
      this.viewAccounts();
    });
  }

  viewAccounts() {
    this.loading = true;
    this.userService.getUserAccounts(this.userId).subscribe((res) => {
      this.userAccountInfo = res;
      this.loading = false;
      this.cdr.markForCheck();
    });
  }

  getUsers() {
    this.loading = true;
    this.userService.getUserList().subscribe((res) => {
      this.users = res;
      this.selecteduser = this.users.find((u) => u.id === this.userId);
      this.loading = false;
      this.cdr.markForCheck();
    });
  }

  createNewAccount() {
    this.loading = true;
    this.cdr.markForCheck();
    this.accountService
      .create(this.userId, this.newAccountInitialAmount)
      .subscribe((res) => {
        this.viewAccounts();
        this.createNewAccountSelected = false;
        this.cdr.markForCheck();
      });
  }

  showTransactions(accountNumber: string) {
    this.selectedAccountTransaction = this.userAccountInfo?.accounts.find(
      (s) => s.accountNumber === accountNumber
    )?.transactions;
    this.selectedAccountTransaction = this.selectedAccountTransaction?.reverse();

    this.selectedAccountNumber = accountNumber;
    this.showDialog = true;
    this.cdr.markForCheck();
  }

  closeDialog() {
    this.showDialog = false;
    this.cdr.markForCheck();
  }

  addMoney() {
    const account: Account | undefined = this.userAccountInfo?.accounts.find(
      (s) => s.accountNumber === this.selectedAccountNumber
    );
    if (account)
      this.accountService
        .addTransaction(account?.id, this.newAccountInitialAmount)
        .subscribe(() => {
          this.addTransactionSelected = false;
          this.showDialog = false;
          this.viewAccounts();
          this.cdr.markForCheck();
        });
  }
}
