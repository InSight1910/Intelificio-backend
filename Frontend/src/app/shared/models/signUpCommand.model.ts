import { singUp } from "./singup.model";

export interface SignupDTO {
    User: singUp;
    Users: singUp[];
    CreatorID: number;
    CommunityID: number;
  }
