import { makeVar } from "@apollo/client"

export const apiErrorVar = makeVar<string | null>(null)
