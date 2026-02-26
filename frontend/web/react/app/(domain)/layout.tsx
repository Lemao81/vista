import type React from 'react';
import AppBar from '@/components/layout/app-bar';
import Footer from '@/components/layout/footer';
import SideNavigation from '@/components/layout/side-navigation';
import ModalProvider from '@/components/shared/modal-provider';
import QueryClientProviderWrapper from '@/components/shared/query-client-provider-wrapper';

export default function DomainLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <QueryClientProviderWrapper>
      <ModalProvider>
        <div className="app-container grid">
          <AppBar />
          <div className="app-outer-body grid">
            <SideNavigation />
            <div className="app-inner-body grid">
              <main className="bg-neutral-200 p-2">{children}</main>
              <Footer />
            </div>
          </div>
        </div>
      </ModalProvider>
    </QueryClientProviderWrapper>
  );
}
