import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  registerMode = false;
  users: any;
  http = inject(HttpClient);


  ngOnInit(): void {
    this.getUser();
  }
  
  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = false;
  }

  getUser(){
    this.http.get('http://localhost:5045/api/users').subscribe({
      next: res => {
        this.users = res;
        console.log(this.users);
      },
      error: e => {
        console.log(e);
      },
      complete: () =>{
        console.log('Acabou');
      }
    })
  }
}
