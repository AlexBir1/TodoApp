import { AttachmentModel } from "./attachment.model";
import { CollectionModel } from "./collection.model";
import { GoalCategory } from "./goal-category.model";

export interface GoalModel{
    id: string;
    title: string;
    description: string;
    isCompleted: boolean;
    startDate: Date;
    collectionId: string;
    collection: CollectionModel;

    creationDate: Date;
    updateDate: Date;

    attachments: AttachmentModel[];
    goalCategories: GoalCategory[];
}