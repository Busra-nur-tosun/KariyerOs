import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-page',
  imports: [RouterLink],
  templateUrl: './home-page.component.html',
})
export class HomePageComponent {
  protected readonly features = [
    {
      icon: 'CV',
      title: 'AI tabanli dinamik CV optimizasyonu',
      description: 'Her is ilani icin yapay zeka tarafindan gercek zamanli duzenlenen, ATS uyumlu CV olustur.',
    },
    {
      icon: 'SG',
      title: 'Skill gap raporu ve mulakat hazirligi',
      description: 'Eksik yeteneklerini belirle, sorulara hazirlan ve guclu yonlerini gorunur hale getir.',
    },
    {
      icon: 'MS',
      title: 'Piyasa verileriyle maas beklentisi normalizasyonu',
      description: 'Gercekci maas beklentileri belirle ve muzakerelere veri destekli hazirlan.',
    },
  ];

  protected readonly matchExamples = [
    { name: 'Ahmet D. - Ileri Yazilim Muhendisi', score: '92%' },
    { name: 'Zeynep Y. - Orta Seviye Full Stack', score: '78%' },
    { name: 'Mustafa K. - Junior Yazilim Gelistirici', score: '64%' },
  ];

  protected readonly localDynamics = [
    { title: 'Universite', text: 'Universite ve bolum bazli degerlendirme' },
    { title: 'Sertifikasyon', text: 'Uluslararasi ve yerel sertifikalar' },
    { title: 'Askerlik Durumu', text: 'Turkiye is piyasasi dinamiklerine uygun filtreleme' },
    { title: 'Yabanci Dil', text: 'Global roller ve yerel beklentiler icin dil seviyesi' },
  ];

  protected readonly steps = [
    {
      number: '1',
      title: 'Is ilanini ekle veya linkini paylas',
      text: 'Basvurmak istedigin pozisyonun ilanini kopyala veya baglantisini yapistir.',
    },
    {
      number: '2',
      title: "AI ile ATS uyumlu CV'ni ve skill gap raporunu al",
      text: 'Dakikalar icinde optimize edilmis CV ve detayli skill analizi gor.',
    },
    {
      number: '3',
      title: 'Mulakat hazirligi ve maas normalizasyonu',
      text: 'Mulakat sorularina hazir ol ve piyasa verilerine dayali maas beklentini netlestir.',
    },
  ];

  protected readonly metrics = [
    { value: '87%', label: 'CV uyum skoru artisi' },
    { value: '3x', label: 'Daha hizli basvuru hazirligi' },
    { value: '24/7', label: 'AI destekli kariyer rehberligi' },
  ];

  protected readonly reviews = [
    {
      quote: 'CVmi her ilana gore yeniden yazmak yerine artik veriyle optimize ediyorum.',
      author: 'Ece K.',
      role: 'Urun Tasarimcisi',
    },
    {
      quote: 'Skill gap analizi sayesinde hangi alanlara yatirim yapmam gerektigini net gordum.',
      author: 'Berk A.',
      role: 'Full Stack Developer',
    },
    {
      quote: 'Mulakat hazirligi ve maas benchmarki ayni yerde oldugu icin surec cok hizlandi.',
      author: 'Selin T.',
      role: 'Product Manager',
    },
  ];

  protected readonly resources = [
    'ATS uyumlu CV rehberi',
    'Mulakat hazirlik notlari',
    'Turkiye maas beklentisi icgoru seti',
  ];
}
