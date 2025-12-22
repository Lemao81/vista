import React from 'react';

export default function SideNavigation() {
  return (
    <nav className="side-nav flex w-64 flex-col gap-1 bg-neutral-300 p-1 pt-4">
      <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-400">
        Link 1
      </a>
      <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-400">
        Link 2
      </a>
      <a href="" className="inline-flex items-center justify-center h-12 bg-neutral-400">
        Link 3
      </a>
    </nav>
  );
}
