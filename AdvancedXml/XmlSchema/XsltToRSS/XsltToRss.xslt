<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:catalog="http://library.by/catalog"
                extension-element-prefixes="catalog">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/catalog:catalog">
    <Catalog>
      <xsl:apply-templates />
    </Catalog>
  </xsl:template>

  <xsl:template match="catalog:book">
    <Book>
      <Title>
        <xsl:value-of select="catalog:title"/>
      </Title>
      <Date>
        <xsl:value-of select="catalog:publish_date"/>
      </Date>
      <Description>
        <xsl:value-of select="catalog:description"/>
      </Description>
      <Link>
        <xsl:if test="(catalog:genre = 'Computer')  and (string-length(catalog:isbn)!=0)">
          <xsl:text>http://my.safarybooksonline.com</xsl:text>
        </xsl:if>
      </Link>
    </Book>
  </xsl:template>

  <xsl:template match="text() | @*"/>

</xsl:stylesheet>
