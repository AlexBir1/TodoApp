import { AttachmentModel } from "./attachment.model";
import { GoalCategory } from "./goal-category.model";

export interface GoalModel{
    id: string;
    title: string;
    description: string;
    isCompleted: boolean;
    startDate: Date;
    collectionId: string;

    creationDate: Date;
    updateDate: Date;

    attachments: AttachmentModel[];
    goalCategories: GoalCategory[];
}