export interface Region {
  id: number;
  name?: string;
}

export interface City {
  id: number;
  name: string;
  region?: Region;
}

export interface Municipality {
  id: number;
  name: string;
  city?: City;
}
