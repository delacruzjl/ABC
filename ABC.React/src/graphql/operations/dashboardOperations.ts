import { gql } from "@apollo/client"

export const DASHBOARD_QUERY = gql`
  query Dashboard {
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
