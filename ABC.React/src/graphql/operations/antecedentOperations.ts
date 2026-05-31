import { gql } from "@apollo/client"

export const GET_ANTECEDENTS = gql`
  query GetAntecedents {
    antecedents {
      nodes {
        id
        name
        description
      }
    }
  }
`

export const GET_TRANSLATED_ANTECEDENTS = gql`
  query GetTranslatedAntecedents {
    translatedAntecedents {
      id
      name
      description
    }
  }
`

export const CREATE_ANTECEDENT = gql`
  mutation CreateAntecedent($name: String!, $description: String!) {
    createAntecedent(name: $name, description: $description) {
      id
      name
      description
    }
  }
`

export const REMOVE_ANTECEDENT = gql`
  mutation RemoveAntecedent($antecedentId: UUID!) {
    removeAntecedent(antecedentId: $antecedentId)
  }
`
