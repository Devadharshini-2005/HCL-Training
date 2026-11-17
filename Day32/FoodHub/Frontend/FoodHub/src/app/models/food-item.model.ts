
export interface FoodItem {
  foodItemId: number;
  foodName: string;
  description: string | null;
  price: number;
  imageUrl: string | null;
  categoryId: number;
  categoryName: string;
}
