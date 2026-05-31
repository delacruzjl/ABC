import { gql } from "@apollo/client"

export const GET_PREFERRED_LANGUAGE = gql`
  query GetPreferredLanguage {
    preferredLanguage
  }
`
