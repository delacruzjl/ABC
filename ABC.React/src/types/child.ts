export interface ChildCondition {
  id: string
  name: string
}

export interface Child {
  id: string
  firstName: string
  lastName: string
  birthYear: number
  userId: string
  conditions: ChildCondition[]
  observationCount: number
}

export interface UserInfo {
  id: string
  email: string
  roles: string[]
}
