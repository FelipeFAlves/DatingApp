import { Component, inject, OnInit } from '@angular/core';
import { LikesService } from '../_service/likes.service';
import { Member } from '../_models/member';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from '../members/member-card/member-card.component';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [ButtonsModule,FormsModule,MemberCardComponent],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit{
  
  private likesService = inject(LikesService);
  members: Member[]= [];
  predicate = 'liked';

  ngOnInit(): void {
    this.loadLikes();
  }

  getTitle(){
    switch (this.predicate){
      case 'liked': return 'Membros que você curtiu';
      case 'likedBy': return 'Membros que curtiram você';
      default: return 'Mutuos'
    }
  }

  loadLikes(){
    this.likesService.getLikes(this.predicate).subscribe({
      next: members => this.members = members
    })
  }




}
