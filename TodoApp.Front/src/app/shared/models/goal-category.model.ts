import { CategoryModel } from "./category.model";
import { GoalModel } from "./goal.model";

export interface GoalCategory{
    goalId: string;
    categoryId: string;

    category: CategoryModel;
    goal: GoalModel;
} 