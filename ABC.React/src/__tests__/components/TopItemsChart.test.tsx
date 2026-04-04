import React from "react"
import { render, screen } from "@testing-library/react"
import "@testing-library/jest-dom"
import { TopItemsChart } from "../../components/TopItemsChart"
import type { ItemWithObservationCount } from "../../types/observation"

// Mock recharts to avoid rendering issues in jsdom
jest.mock("recharts", () => {
  const OriginalModule = jest.requireActual("recharts")
  return {
    ...OriginalModule,
    ResponsiveContainer: ({ children }: { children: React.ReactNode }) => (
      <div data-testid="responsive-container">{children}</div>
    ),
  }
})

const mockItems: ItemWithObservationCount[] = [
  { id: "1", name: "Loud noise", observationCount: 5 },
  { id: "2", name: "Crowded room", observationCount: 3 },
  { id: "3", name: "Quiet space", observationCount: 1 },
]

describe("TopItemsChart", () => {
  it("renders the title", () => {
    render(<TopItemsChart title="Top Antecedents" items={mockItems} />)
    expect(screen.getByText("Top Antecedents")).toBeInTheDocument()
  })

  it("shows empty message when no items", () => {
    render(<TopItemsChart title="Top Antecedents" items={[]} />)
    expect(screen.getByText("No data available")).toBeInTheDocument()
  })

  it("renders the chart container when items exist", () => {
    render(<TopItemsChart title="Top Antecedents" items={mockItems} />)
    expect(screen.getByTestId("responsive-container")).toBeInTheDocument()
  })
})
