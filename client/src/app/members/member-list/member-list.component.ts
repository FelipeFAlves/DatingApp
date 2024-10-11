import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_service/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from '../member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_service/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons'


@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent,PaginationModule,FormsModule,ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit{
  private accountService = inject(AccountService);
  memberService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [{value: 'male',display:'Masculino'}, {value: 'female', display: 'Feminino'}]

  
  ngOnInit(): void {
    if(!this.memberService.paginatedResult()) this.loadMembers();
  }
  loadMembers(){
    this.memberService.getMembers(this.userParams);
  }

  resetFilters(){
    this.userParams = new UserParams(this.accountService.currentUser());
    this.loadMembers();
  }
  pageChanged(event: any){
    if(this.userParams.pageNumber !== event.page){
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
