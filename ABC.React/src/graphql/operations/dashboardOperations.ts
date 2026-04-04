import { gql } from "@apollo/client"

export const GET_ANTECEDENTS_WITH_OBSERVATIONS = gql`
  query GetAntecedentsWithObservations {
    antecedents(first: 100) {
      nodes {
        id
        name
        observations {
          id
          child {
            id
            firstName
            lastName
          }
        }
      }
    }
  }
`

export const GET_BEHAVIORS_WITH_OBSERVATIONS = gql`
  query GetBehaviorsWithObservations {
    behaviors(first: 100) {
      nodes {
        id
        name
        observations {
          id
          child {
            id
            firstName
            lastName
          }
        }
      }
    }
  }
`

export const GET_CONSEQUENCES_WITH_OBSERVATIONS = gql`
  query GetConsequencesWithObservations {
    consequences(first: 100) {
      nodes {
        id
        name
        observations {
          id
          child {
            id
            firstName
            lastName
          }
        }
      }
    }
  }
`

export const GET_RECENT_OBSERVATIONS = gql`
  query GetRecentObservations {
    observations(first: 50) {
      nodes {
        id
        notes
        status
        when {
          startedAt
          endedAt
        }
        antecedents {
          id
          name
        }
        behaviors {
          id
          name
        }
        consequences {
          id
          name
        }
        child {
          id
          firstName
          lastName
        }
      }
    }
  }
`
