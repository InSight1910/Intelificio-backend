export interface Email {
  Subject: string;
  PreHeader: string;
  Title: string;
  Message: string;
  SenderName: string;
  CommunityID: number;
  BuildingID?: number;
  Floor?: number;
}
