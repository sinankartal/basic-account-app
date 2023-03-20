import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UserService } from 'src/service/user-service';
import { User, UserAccountsDTO } from 'src/model/models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  users: User[] | undefined;
  userAccountInfo: UserAccountsDTO | undefined;
  selectedUser!: User;

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.getUserList().subscribe((res) => {
      this.users = res;
      if (this.users?.length > 0) this.selectedUser = this.users[0];
      this.cdr.markForCheck();
    });
  }

  viewAccounts(userId: string) {
    this.router.navigate(['/user-list', userId]);
  }
}
