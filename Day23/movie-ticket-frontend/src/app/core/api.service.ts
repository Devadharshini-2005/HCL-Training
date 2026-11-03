import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Movie, Theatre, Screen, Seat, Show, ShowSeat, Booking, CreateBookingRequest, RegisterRequest, LoginRequest, AuthResponse } from './models';
@Injectable({providedIn:'root'})
export class ApiService {
  private b=environment.apiUrl;
  constructor(private http:HttpClient){}
  get<T>(path:string,query:Record<string,string|number|undefined|null>={}):Observable<T>{let p=new HttpParams();Object.entries(query).forEach(([k,v])=>{if(v!==undefined&&v!==null&&v!=='')p=p.set(k,String(v));});return this.http.get<T>(this.b+path,{params:p});}
  post<T>(path:string,body:unknown):Observable<T>{return this.http.post<T>(this.b+path,body);}
  put<T>(path:string,body:unknown={}):Observable<T>{return this.http.put<T>(this.b+path,body);}
  delete<T>(path:string):Observable<T>{return this.http.delete<T>(this.b+path);}
  // Auth endpoints
  register(body:RegisterRequest){return this.post<unknown>('/Auth/register',body);}
  login(body:LoginRequest){return this.post<AuthResponse>('/Auth/login',body);}
  // Movies endpoints
  getMovies(pageNumber=1,pageSize=10,title?:string){return this.get<Movie[]>('/movies',{pageNumber,pageSize,title});}
  getMovie(id:number){return this.get<Movie>('/movies/'+id);}
  searchMovies(title:string){return this.get<Movie[]>('/movies/search',{title});}
  createMovie(body:Partial<Movie>){return this.post<Movie>('/movies',body);}
  updateMovie(id:number,body:Partial<Movie>){return this.put<Movie>('/movies/'+id,body);}
  deleteMovie(id:number){return this.delete<void>('/movies/'+id);}
  // Theatres endpoints
  getTheatres(){return this.get<Theatre[]>('/Theatres');}
  getTheatre(id:number){return this.get<Theatre>('/Theatres/'+id);}
  createTheatre(body:Partial<Theatre>){return this.post<Theatre>('/Theatres',body);}
  updateTheatre(id:number,body:Partial<Theatre>){return this.put<Theatre>('/Theatres/'+id,body);}
  deleteTheatre(id:number){return this.delete<void>('/Theatres/'+id);}
  // Screens endpoints
  getScreens(theatreId?:number){return this.get<Screen[]>('/Screens',{theatreId});}
  getScreen(id:number){return this.get<Screen>('/Screens/'+id);}
  getScreensByTheatre(theatreId:number){return this.get<Screen[]>('/Screens/theatre/'+theatreId);}
  createScreen(body:Partial<Screen>){return this.post<Screen>('/Screens',body);}
  updateScreen(id:number,body:Partial<Screen>){return this.put<Screen>('/Screens/'+id,body);}
  deleteScreen(id:number){return this.delete<void>('/Screens/'+id);}
  // Seats endpoints
  getSeat(id:number){return this.get<Seat>('/Seats/'+id);}
  getSeatsByScreen(screenId:number){return this.get<Seat[]>('/Seats/screen/'+screenId);}
  createSeat(body:Partial<Seat>){return this.post<Seat>('/Seats',body);}
  createSeatsBulk(seats:unknown[]){return this.post<Seat[]>('/Seats/bulk',{seats});}
  updateSeat(id:number,body:Partial<Seat>){return this.put<Seat>('/Seats/'+id,body);}
  deleteSeat(id:number){return this.delete<void>('/Seats/'+id);}
  // Shows endpoints
  getShows(filters:{date?:string;movieId?:number;theatreId?:number}={}){return this.get<Show[]>('/shows',filters);}
  getShow(id:number){return this.get<Show>('/shows/'+id);}
  getShowsByMovie(movieId:number){return this.get<Show[]>('/shows/movie/'+movieId);}
  getShowsByTheatre(theatreId:number){return this.get<Show[]>('/shows/theatre/'+theatreId);}
  searchShows(date:string){return this.get<Show[]>('/shows/search',{date});}
  getShowSeats(showId:number){return this.get<ShowSeat[]>('/shows/'+showId+'/seats');}
  getAvailableShowSeats(showId:number){return this.get<ShowSeat[]>('/shows/'+showId+'/seats/available');}
  getBookedShowSeats(showId:number){return this.get<ShowSeat[]>('/shows/'+showId+'/seats/booked');}
  createShow(body:Partial<Show>){return this.post<Show>('/shows',body);}
  updateShow(id:number,body:Partial<Show>){return this.put<Show>('/shows/'+id,body);}
  deleteShow(id:number){return this.delete<void>('/shows/'+id);}
  // Bookings endpoints
  createBooking(body:CreateBookingRequest){return this.post<Booking>('/Bookings',body);}
  getAllBookings(showId?:number,status?:string){return this.get<Booking[]>('/Bookings',{showId,status});}
  getBooking(id:number){return this.get<Booking>('/Bookings/'+id);}
  getBookingsByShow(showId:number){return this.get<Booking[]>('/Bookings/show/'+showId);}
  cancelBooking(id:number){return this.put<Booking>('/Bookings/'+id+'/cancel',{});}
}
