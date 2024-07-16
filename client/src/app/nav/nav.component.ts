import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_service/account.service';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,CommonModule,BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountServie = inject(AccountService);
  model:any ={

  }
  
  login(){
    this.accountServie.login(this.model).subscribe({
      next: response =>{
        console.log(response);
      },
      error: error => console.log(error),
    })
  }
  logout(){
    this.accountServie.logut()
  }
}
