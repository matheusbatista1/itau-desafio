// path: src/components/CardSummary.tsx
import React from "react";

interface CardSummaryProps {
  title: string;
  value: React.ReactNode;
}

export function CardSummary({ title, value }: CardSummaryProps) {
  return (
    <div className='border rounded-lg p-4 shadow-sm bg-white'>
      <h3 className='text-sm text-gray-500'>{title}</h3>
      <p className='text-lg font-bold'>
        {React.isValidElement(value) ? value : String(value)}
      </p>
    </div>
  );
}
