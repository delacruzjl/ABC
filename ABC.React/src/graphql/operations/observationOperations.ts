import { gql } from "@apollo/client"

export const START_OBSERVATION = gql`
  mutation StartObservation($childId: UUID!) {
    startObservation(childId: $childId) {
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
`

export const UPDATE_OBSERVATION = gql`
  mutation UpdateObservation(
    $observationId: UUID!
    $antecedents: [UUID!]
    $behaviors: [UUID!]
    $consequences: [UUID!]
    $notes: String
  ) {
    updateObservation(
      command: {
        observationId: $observationId
        antecedents: $antecedents
        behaviors: $behaviors
        consequences: $consequences
        notes: $notes
      }
    ){
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
    }
  }
`

export const END_OBSERVATION = gql`
  mutation EndObservation($observationId: UUID!) {
    endObservation(observationId: $observationId) {
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
    }
  }
`
