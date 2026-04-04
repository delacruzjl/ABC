import { gql } from "@apollo/client"

export const GET_BEHAVIORS = gql`
  query GetBehaviors {
    behaviors {
      nodes {
        id
        name
        description
      }
    }
  }
`

export const CREATE_BEHAVIOR = gql`
  mutation CreateBehavior($name: String!, $description: String!) {
    createBehavior(name: $name, description: $description) {
      id
      name
      description
    }
  }
`

export const REMOVE_BEHAVIOR = gql`
  mutation RemoveBehavior($behaviorId: UUID!) {
    removeBehavior(behaviorId: $behaviorId)
  }
`
