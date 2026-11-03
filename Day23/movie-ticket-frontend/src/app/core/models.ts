export interface AuthResponse { userId:number; fullName:string; email:string; role:string; token:string; expiresAt:string; }
export interface RegisterRequest { fullName:string; email:string; password:string; phoneNumber?:string|null; }
export interface LoginRequest { email:string; password:string; }
export interface Movie { movieId:number; title:string; description:string|null; durationMinutes:number; releaseDate:string; language:string; genre:string; }
export interface Theatre { theatreId:number; name:string; location:string; }
export interface Screen { screenId:number; theatreId:number; screenName:string; }
export interface Seat { seatId:number; screenId:number; rowName:string; seatNumber:number; seatType:string; }
export interface Show { showId:number; movieId:number; screenId:number; startTime:string; endTime:string; ticketPrice:number; }
export interface ShowSeat { showSeatId:number; showId:number; seatId:number; rowName:string; seatNumber:number; seatType:string; status:string; }
export interface CreateBookingRequest { showId:number; customerName:string; customerEmail:string; customerPhone:string; seatIds:number[]; }
export interface Booking { bookingId:number; bookingReference:string; showId:number; totalAmount:number; status:string; seats:string[]; }
